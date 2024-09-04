using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web;
using To_Do_List.Models;

public class AuthController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly EmailSender _emailSender; // Добавление EmailSender

    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, EmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender; // Инициализация EmailSender
    }

    // GET: Auth/Register
    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    // POST: Auth/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser == null)
            {
                // Если пользователя не существует, создаем нового
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Генерация токена для подтверждения почты
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedToken = HttpUtility.UrlEncode(token);
                    var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = encodedToken }, Request.Scheme);

                    // Отправка письма для подтверждения почты
                    await _emailSender.SendEmailAsync(model.Email, "Подтверждение почты", $"Подтвердите вашу почту, перейдя по ссылке: <a href='{confirmationLink}'>link</a>");

                    // Уведомление пользователя о том, что письмо отправлено
                    ModelState.AddModelError(string.Empty, "На вашу почту отправлено письмо для подтверждения почты.");
                    return View(model);
                }

                // Обработка ошибок при создании пользователя
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                // Если пользователь существует, проверяем подтверждение почты
                if (!existingUser.EmailConfirmed)
                {
                    // Генерация токена для подтверждения почты
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(existingUser);
                    var encodedToken = HttpUtility.UrlEncode(token);
                    var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = existingUser.Id, token = encodedToken }, Request.Scheme);

                    // Отправка письма для подтверждения почты
                    await _emailSender.SendEmailAsync(existingUser.Email!, "Подтверждение почты", $"Подтвердите вашу почту, перейдя по ссылке: <a href='{confirmationLink}'>link</a>");

                    // Уведомление пользователя о повторной отправке письма
                    ModelState.AddModelError(string.Empty, "Письмо для подтверждения почты было отправлено повторно.");
                    return View(model);
                }
                else
                {
                    // Если почта уже подтверждена, перенаправляем на главную страницу
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        // Возвращаем представление с сообщением об ошибках валидации, если есть
        return View(model);
    }

    // GET: Auth/ConfirmEmail
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Не удалось найти пользователя с идентификатором '{userId}'.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            // Успешное подтверждение, перенаправляем на страницу с сообщением
            return RedirectToAction("EmailConfirmed", "Auth");
        }

        // Если не удалось, возвращаем страницу с ошибкой
        return NotFound("Ошибка подтверждения почты.");
    }

    // GET: Auth/EmailConfirmed
    public IActionResult EmailConfirmed()
    {
        return View(); // Вернуть представление EmailConfirmed
    }


    // GET: Auth/Login
    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    // POST: Auth/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Необходимо подтвердить вашу почту.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Неверная попытка входа.");
        }
        return View(model);
    }

    // GET: Auth/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
