export const getCSRF = () => {
  const input = document.querySelector(
    'input[name="__RequestVerificationToken"]'
  ) as HTMLInputElement | null;
  if (!input) return "";
  return input.value;
};
