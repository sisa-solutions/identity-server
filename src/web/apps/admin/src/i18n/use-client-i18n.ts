'use client';

import { useEffect, useState } from 'react';

import type { FlatNamespace, KeyPrefix } from 'i18next';

import { useTranslation, type FallbackNs, type UseTranslationOptions } from 'react-i18next';
import { useParams } from 'next/navigation';

type $Tuple<T> = readonly [T?, ...T[]];

const runsOnServerSide = typeof window === 'undefined';

const useClientI18n = <
  Ns extends FlatNamespace | $Tuple<FlatNamespace> | undefined = undefined,
  KPrefix extends KeyPrefix<FallbackNs<Ns>> = undefined,
>(
  ns?: Ns,
  options?: UseTranslationOptions<KPrefix>
) => {
  const { lng } = useParams<{
    lng: string;
  }>();
  const ret = useTranslation(ns, options);

  const [translation] = useState(ret);
  const [, i18n] = translation;

  if (runsOnServerSide && i18n.resolvedLanguage !== lng) {
    i18n.changeLanguage(lng);
  }

  useEffect(() => {
    if (!runsOnServerSide && i18n.resolvedLanguage !== lng) {
      i18n.changeLanguage(lng);
    }

    // setTranslation((prev) => {
    //   return [
    //     prev.i18n.getFixedT(lng, Array.isArray(ns) ? ns[0] : ns, options?.keyPrefix),
    //     prev.i18n,
    //     prev[3],
    //   ];
    // });
  }, [i18n, lng, ns, options?.keyPrefix]);

  return ret;
};

export default useClientI18n;
