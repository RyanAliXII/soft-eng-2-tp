const toastSuccess = document.querySelector("#toast-success");

const toast = {
  success: (message) => {
    const toastSuccessClone = toastSuccess.content.cloneNode(true);
    toastSuccessClone.querySelector("#message").innerText = message;
    document.body.appendChild(toastSuccessClone);
  },
};
