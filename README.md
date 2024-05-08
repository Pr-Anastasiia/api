# Прототип власного апі з використання апі Google Calendar
Бажанні функції: 1) залогінитись під своєю поштою
                 2) створити подію
                 3) позначити як виконану
                 4) змістити подію по часу
                 5) видалити подію
                 6) отримати список подій на певний проміжок часу

Наразі Swagger UI стабільно викидає помилку:
System.InvalidOperationException: Endpoint api.Controllers.AuthController.GoogleLogin (api) contains CORS metadata, but a middleware was not found that supports CORS.
Configure your application startup by adding app.UseCors() in the application startup code. If there are calls to app.UseRouting() and app.UseEndpoints(...), the call to app.UseCors() must go between them.
   at Microsoft.AspNetCore.Routing.EndpointMiddleware.ThrowMissingCorsMiddlewareException(Endpoint endpoint)
   at Microsoft.AspNetCore.Routing.EndpointMiddleware.Invoke(HttpContext httpContext)
   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)

!! Доки увесь функціонал не є client friendly, бажано змінити поля calendarId та eventId на більш зручніші для користувача. Певні контролери ще знаходяться на стадії розробки, до прикладу контролер створення події не вимагає ім'я для неї (що не має відповідати дійсності), оскільки в першу чергу я сконцетрувалась на успішній реалізації авторизації користувачів через Google.
