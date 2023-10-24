import dayUtils from '../day-utils';

import i18next, { type Resource, type TFunction } from 'i18next';
import { initReactI18next } from 'react-i18next/initReactI18next';

import LanguageDetector from 'i18next-browser-languagedetector';

const createI18n = (
  resources: Resource,
  fallbackLng: string,
  languages?: string[],
  defaultNS?: string,
  callback?: (error: any, t: TFunction) => void
) => {
  i18next
    // detect user language
    // learn more: https://github.com/i18next/i18next-browser-languageDetector
    .use(LanguageDetector)
    // pass the i18n instance to react-i18next.
    .use(initReactI18next)
    // init i18next
    // for all options read: https://www.i18next.com/overview/configuration-options
    .init(
      {
        resources,
        fallbackLng,
        supportedLngs: languages,
        defaultNS,

        // detection: {
        //   order: ['path', 'htmlTag', 'cookie', 'navigator'],
        // },

        cleanCode: false,

        interpolation: {
          escapeValue: false, // react already safes from xss => https://www.i18next.com/translation-function/interpolation#unescape
        },

        returnNull: false,
      },
      callback
    );

  i18next.on('languageChanged', (lng) => {
    dayUtils.locale(lng);
  });

  return i18next;
};

export default createI18n;
