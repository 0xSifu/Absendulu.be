using System.Net;
using System.Net.Mail;
using AbsenDulu.BE.DTO;
using AbsenDulu.BE.Interfaces.IServices.Activation;
using AbsenDulu.BE.Interfaces.IServices.Email;
using AbsenDulu.BE.Models.Activation;
using AbsenDulu.BE.Models.Identity;

namespace AbsenDulu.BE.Services.Email;
public class EmailService : IEmailService
{
    private readonly IUserActivationService _userActivation;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration, IUserActivationService userActivation, IHttpContextAccessor httpContext)
    {
        _userActivation = userActivation;
        _httpContextAccessor = httpContext;
        _configuration = configuration;
    }
    public Task SendEmailAsync(UserRegisterDTO user, string subject, Guid userId)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var root = $"{httpContext?.Request.Scheme}://{httpContext?.Request.Host}";
        string activationCode = Guid.NewGuid().ToString();
        var activationLink = $"{root}/auth/active/{user.UserName}/{activationCode}";
        var activationClient = $"{root}/active/{user.UserName}/{activationCode}";
        UserActivation activation = new UserActivation
        {
            ActivationCode = activationCode,
            IsActivated = false,
            UserId = userId,
            ExpireTime = DateTime.UtcNow.AddMinutes(5),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };
        _userActivation.AddActivation(activation);
        var client = new SmtpClient("mail.absendulu.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("development@absendulu.com", "nR&d)XwP$,L.")
        };
        var mailMessage = new MailMessage(from: "development@absendulu.com",
            to: user.Email,
            subject,
            body: GetBody(activationLink, user.UserName, activationClient))
        {
            IsBodyHtml = true
        };
        return client.SendMailAsync(mailMessage);
    }

    public Task SendEmaiResetPassword(UserResetPassword dataResetPassword, string Email, string Subject)
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var root = $"{httpContext?.Request.Scheme}://{httpContext?.Request.Host}";
            var ResetLink = $"{root}/auth/ChangePassword/{dataResetPassword.ResetPasswordCode}";
            var client = new SmtpClient("mail.absendulu.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("development@absendulu.com", "nR&d)XwP$,L.")
            };
            var mailMessage = new MailMessage(from: "development@absendulu.com",
                to: Email,
                Subject,
                body: GetBodyResetPassword(ResetLink, dataResetPassword.PinCode))
            {
                IsBodyHtml = true
            };
            return client.SendMailAsync(mailMessage);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private string GetBody(string linkActivation, string username, string activationClient)
    {
        string body = $@"
                            <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Email Confirmation</title>
                    <style>
                        /* Gaya email */
                        body {{
                            font-family: Arial, sans-serif;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                            background-color: #f5f5f5;
                            border-radius: 5px;
                        }}
                        h2 {{
                            color: #333333;
                            margin-top: 0;
                        }}
                          .btn {{
                            display: inline-block;
                            padding: 10px 20px;
                            background-color: #29999b;
                            color: #ffffff;
                            text-decoration: none;
                            border-radius: 3px;
                            margin-top: 20px;
                        }}
                        p {{
                            color: #333333;
                        }}
                        .footer {{
                            margin-top: 40px;
                            text-align: center;
                            color: #999999;
                        }}
                    </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>Dear {username},</h2>

                            <p>Thank you for registering on our website! We're excited to have you as part of our community. To complete your registration, please click the button below to verify your email address:</p>

                            <a href='{linkActivation}' class='btn'>Verify Email</a>

                            <p><strong>Why verify your email address?</strong></p>
                            <ul>
                                <li>Receive important updates and notifications</li>
                                <li>Reset your password if needed</li>
                                <li>Unlock additional features and personalized experiences</li>
                            </ul>

                            <p><strong>Haven't registered on our website?</strong></p>
                            <p>If you did not sign up for an account on our platform, please disregard this email. No further action is required.</p>

                            <p>Please note:</p>
                            <ul>
                                <li>This link will expire in <strong>30 Minutes </strong> [{DateTime.UtcNow.AddMinutes(30)}].</li>
                                <li>If the button above doesn't work, you can copy and paste the following link into your browser:</li>
                            </ul>

                            <p>{linkActivation}</p>

                            <p>If you have any questions or need assistance, please don't hesitate to reach out to our support team at [Support Email]. We're here to help!</p>

                            <p>Thank you,</p>
                            <p>Absen Dulu IT Team</p>
                        </div>
                    </body>
                </html>";
        return body;
    }

    private string GetBodyResetPassword(string resetLink, string PinCode)
    {
        string body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    .container {{
                        max-width: 600px;
                        margin: 0 auto;
                        padding: 20px;
                        font-family: Arial, sans-serif;
                    }}
                    .logo {{
                        text-align: center;
                        margin-bottom: 20px;
                    }}
                    .logo img {{
                        max-width: 200px;
                    }}
                    .content {{
                        background-color: #f9f9f9;
                        padding: 20px;
                        border-radius: 5px;
                    }}
                    .reset-link {{
                        margin-top: 30px;
                        text-align: center;
                    }}
                    .pin-code {{
                        text-align: center;
                        margin-top: 30px;
                        font-size: 24px;
                        font-weight: bold;
                    }}
                    .footer {{
                        margin-top: 30px;
                        text-align: center;
                        color: #888888;
                    }}
                </style>
            </head>
                <body>
                    <div class='container'>
                        <div class='logo'>
                            <img src='http://103.181.182.144:8000/img/logos/icon_logo.webp' alt='Logo'>
                        </div>
                        <div class='content'>
                            <h2>Reset Password</h2>
                            <p>To reset your password, please copy this PIN on below then input PIN on page change password</p>
                            <p class='pin-code'>Your PIN code: {PinCode}</p>
                            <p><strong>Haven't Reset Password on our website?</strong></p>
                            <p>please disregard this email. No further action is required.</p>

                            <p>Please note:</p>
                            <ul>
                                <li>This link will expire in <strong>30 Minutes</strong>[{DateTime.UtcNow.AddMinutes(30)}].</li>
                                <li>If have Any Problem please contact us on support@absendulu.com</li>
                            </ul>
                        </div>
                        <div class='footer'>
                            © 2023 <strong>Absen Dulu IT Team </strong> All rights reserved.
                        </div>
                    </div>
                </body>
            </html>";

        return body;
    }
    private string GenerateRandomPin()
    {
        Random random = new Random();
        int pin = random.Next(100000, 999999);
        return pin.ToString("D6");
    }

}
