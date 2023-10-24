'use client';

import { useCallback, useState } from 'react';

const useToggle = (initialValue = false) => {
  const [value, setValue] = useState(initialValue);

  const toggle = useCallback(() => setValue((v) => !v), []);

  const on = useCallback(() => setValue(true), []);
  const off = useCallback(() => setValue(false), []);

  return { value, toggle, on, off };
};

export default useToggle;
