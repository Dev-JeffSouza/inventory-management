
document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector(".validate-forms");

    if (form) {
        form.addEventListener("submit", function (event) {
            const requiredFields = form.querySelectorAll("input[required], select[required]");
            let isValid = true;

            // Limpa os erros anteriores de todos os campos
            requiredFields.forEach(field => {
                field.classList.remove("input-error");
            });

            // Verifica cada campo obrigatório
            requiredFields.forEach(field => {
                if (!field.value.trim()) {
                    isValid = false;
                    field.classList.add("input-error");
                }
            });

            // Se houver campos inválidos, cancela a submissão e foca no primeiro erro
            if (!isValid) {
                event.preventDefault();

                const firstError = form.querySelector(".input-error");
                if (firstError) {
                    firstError.focus();
                }
            }
        });
    }
});