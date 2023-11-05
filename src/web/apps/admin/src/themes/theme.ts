import { Public_Sans } from 'next/font/google';

import { extendTheme } from '@mui/joy/styles';

const fonts = Public_Sans({
  weight: ['300', '400', '500', '600', '700'],
  style: ['normal', 'italic'],
  subsets: ['latin', 'vietnamese'],
  display: 'swap',
});

const theme = extendTheme({
  fontFamily: {
    body: fonts.style.fontFamily,
  },
});

export default theme;
