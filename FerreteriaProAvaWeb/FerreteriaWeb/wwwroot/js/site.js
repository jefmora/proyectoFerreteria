document.addEventListener("DOMContentLoaded", function () {

    // Cambio de cantidad en productos
    document.querySelectorAll("[data-cantidad-control]").forEach(function (control) {

        const input = control.querySelector("[data-cantidad-input]");
        const scope = control.closest("form") || control;
        const hidden = scope.querySelector("[data-cantidad-hidden]");

        if (!input || !hidden) {
            return;
        }

        control.querySelectorAll("[data-cantidad-btn]").forEach(function (button) {

            button.addEventListener("click", function () {

                const valor = parseInt(button.dataset.cantidadBtn);
                let cantidad = parseInt(input.value) || 1;

                cantidad += valor;

                if (cantidad < 1) {
                    cantidad = 1;
                }

                input.value = cantidad;
                hidden.value = cantidad;
            });
        });

        input.addEventListener("change", function () {

            let cantidad = parseInt(input.value) || 1;

            if (cantidad < 1) {
                cantidad = 1;
            }

            input.value = cantidad;
            hidden.value = cantidad;
        });
    });


    // Mostrar / ocultar contraseñas
    document.querySelectorAll("[data-toggle-password]").forEach(function (button) {

        button.addEventListener("click", function () {

            const inputId = button.dataset.togglePassword;
            const input = document.getElementById(inputId);

            if (!input) {
                return;
            }

            if (input.type === "password") {
                input.type = "text";
            } else {
                input.type = "password";
            }
        });
    });


    // Confirmaciones
    document.querySelectorAll("[data-confirm]").forEach(function (element) {

        element.addEventListener("click", function (event) {

            const mensaje = element.dataset.confirm;

            if (!confirm(mensaje)) {
                event.preventDefault();
            }
        });
    });


    // Validación de cambio de contraseña
    const formularioPassword =
        document.getElementById("formCambiarContrasenna");

    if (formularioPassword) {

        formularioPassword.addEventListener("submit", function (event) {

            const password =
                document.getElementById("txtContrasenna");

            const confirmPassword =
                document.getElementById("txtConfirmarContrasenna");

            const error =
                document.getElementById("errorPassword");

            if (password && confirmPassword && password.value !== confirmPassword.value) {

                event.preventDefault();

                if (error) {
                    error.classList.remove("d-none");
                }

                return;
            }

            if (error) {
                error.classList.add("d-none");
            }
        });
    }

});
