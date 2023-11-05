export const runtime = 'nodejs'; // 'nodejs' (default) | 'edge'

import { type ReactNode } from 'react';

import { Metadata } from 'next';

import { supportedLngs } from 'i18n/configs';

import App from 'components/shared/app';

export const metadata: Metadata = {
  metadataBase: new URL('https://sisa.cafe'),
};

type Props = {
  children: ReactNode;
  params: {
    lng: string;
  };
};

export async function generateStaticParams() {
  return supportedLngs.map((lng) => ({ lng }));
}

const Layout = ({ children, params: { lng } }: Props) => {
  return (
    <html lang={lng} suppressHydrationWarning>
      <body>
        <App>{children}</App>
      </body>
    </html>
  );
};

export default Layout;
