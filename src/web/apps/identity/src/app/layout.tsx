import { type ReactNode } from 'react';

import { Metadata } from 'next';

import App from 'components/shared/app';

export const metadata: Metadata = {
  metadataBase: new URL('https://id.sisa.io'),
};

type Props = {
  children: ReactNode;
};

const Layout = ({ children }: Props) => {
  return (
    <html lang="en" suppressHydrationWarning>
      <body>
        <App>{children}</App>
      </body>
    </html>
  );
};

export default Layout;
