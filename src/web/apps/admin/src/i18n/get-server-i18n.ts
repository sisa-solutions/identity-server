import { cookies } from 'next/headers';

import { createInstance } from 'i18next';
import { initReactI18next } from 'react-i18next/initReactI18next';
import resourcesToBackend from 'i18next-resources-to-backend';

import { resources, defaultNS, fallbackLng, supportedLngs } from './configs';

const initI18next = async (lng: string, ns?: string | readonly string[]) => {
  const i18nInstance = createInstance();
  await i18nInstance
    .use(initReactI18next)
    .use(
      resourcesToBackend((language: string, namespace?: string) => {
        if (namespace) return import(`./locales/${language}/${namespace}.json`);
        else return import(`./locales/${language}.json`);
      })
    )
    .init({
      resources,
      fallbackLng,
      supportedLngs,
      defaultNS,
      ns,
      lng,

      // detection: {
      //   order: ['path', 'htmlTag', 'cookie', 'navigator'],
      // },

      cleanCode: false,

      interpolation: {
        escapeValue: false, // react already safes from xss => https://www.i18next.com/translation-function/interpolation#unescape
      },

      returnNull: false,
    });
  return i18nInstance;
};

const getServerI18n = async (ns?: string | readonly string[]) => {
  const cookieStore = cookies();
  const lng = cookieStore.get('lng')?.value ?? 'en';
  const i18n = await initI18next(lng, ns);

  return {
    t: i18n.t,
    i18n,
  };
};

export default getServerI18n;
