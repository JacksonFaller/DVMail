using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using Mail.Logger;

namespace Mail.API
{
    public class ExceptionHandling : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ArgumentException)
            {
                context.Response =
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(context.Exception.Message)
                    };
                Logger.Logger.Instance.Error($"{context.Exception.Message}");
            }

            SqlException ex = context.Exception as SqlException;
            if (ex != null)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                string message;

                switch (ex.Number)
                {
                    case 2:
                        {
                            message = "Отсутствует подключение к базе данных";
                            break;
                        }
                    case -1:
                        {
                            message = "Ошибка поключения к базе данных. Сервер не найден или не доступен.";
                            break;
                        }

                    case 2627:
                        {
                            message = "Такой элемент уже сушествует";
                            break;
                        }
                    default:
                        {
                            message =
                                $"Необработанная ошибка при обращении к базе данных:{Environment.NewLine}{ex.Message}";
                            break;
                        }
                }
                Logger.Logger.Instance.Error(context.Exception.Message);
                context.Response.Content = new StringContent(message);
            }
        }
    }
}