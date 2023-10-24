import { useCallback, useEffect } from 'react';

import { useRouter, usePathname, useSearchParams } from 'next/navigation';

const useQueryString = <QueryObject extends Record<string, string | number>>(
  initParams?: QueryObject
) => {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  useEffect(() => {
    if (initParams) {
      const params = new URLSearchParams(searchParams);

      Object.keys(initParams).forEach((key) => {
        const value = initParams[key];

        if (value) {
          params.set(key, value.toString());
        }
      });

      router.push(`${pathname}?${params.toString()}`);
    }
  }, [initParams]);
  // Get a new searchParams string by merging the current
  // searchParams with a provided key/value pair
  const setQueryString = useCallback(
    (newParams: Partial<QueryObject>) => {
      const params = new URLSearchParams(searchParams);

      Object.keys(newParams).forEach((key) => {
        const value = newParams[key];

        if (value) {
          params.set(key, value.toString());
        } else {
          params.delete(key);
        }
      });

      router.push(`${pathname}?${params.toString()}`);
    },
    [searchParams]
  );

  return setQueryString;
};

export default useQueryString;
