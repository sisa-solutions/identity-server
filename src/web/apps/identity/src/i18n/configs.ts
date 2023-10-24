import en from './locales/en.json';
import vi from './locales/vi.json';

export const cookieName = 'lng';
export const fallbackLng = 'en';
export const defaultNS = 'translation';
export const supportedLngs = ['en', 'vi'];

export const resources = {
  en: {
    translation: en,
  },
  vi: {
    translation: vi,
  },
};
