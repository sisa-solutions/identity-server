const useCssVariables = (name: string) => {
  const setValue = (value: string) => {
    document.documentElement.style.setProperty(name, value);
  };

  const getValue = () => {
    return window.getComputedStyle(document.documentElement).getPropertyValue(name);
  };

  const destroy = () => {
    document.documentElement.style.removeProperty(name);
  };

  return { getValue, setValue, destroy };
};

export default useCssVariables;
