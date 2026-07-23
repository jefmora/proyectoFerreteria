using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FerreteriaWeb.Filter
{
    public class EsAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var autenticado = context.HttpContext.Session.GetString("Autenticado");
            var consecutivoRol = context.HttpContext.Session.GetInt32("ConsecutivoRol");

            if (autenticado != "1" || consecutivoRol != 1) // 1 = Administrador
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
