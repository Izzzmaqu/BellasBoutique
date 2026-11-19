const modalConfirmacion = document.getElementById("modal-confirmacion");
const mensajeDelModal = modalConfirmacion.querySelector("[data-modal-mensaje]");
let formularioPorConfirmar = null;

document.addEventListener("submit", (evento) => {
    const formulario = evento.target;
    const mensaje = formulario.dataset.confirmar;
    if (!mensaje) {
        return;
    }

    evento.preventDefault();
    formularioPorConfirmar = formulario;
    mensajeDelModal.textContent = mensaje;
    modalConfirmacion.returnValue = "";
    modalConfirmacion.showModal();
});

modalConfirmacion.addEventListener("close", () => {
    if (modalConfirmacion.returnValue === "confirmar") {
        formularioPorConfirmar.submit();
    }
    formularioPorConfirmar = null;
});
