import { NextRequest, NextResponse } from 'next/server';
import acceptLanguage from 'accept-language';
import { fallbackLng, supportedLngs, cookieName } from 'i18n/configs';

acceptLanguage.languages(supportedLngs);

export const config = {
  // matcher: '/:lng*'
  matcher: ['/((?!api|_next/static|_next/image|assets|favicon.ico|sw.js).*)'],
};

export function middleware(req: NextRequest) {
  let lng: string | null = '';

  if (req.cookies.has(cookieName)) lng = acceptLanguage.get(req.cookies.get(cookieName)?.value);

  if (!lng) lng = acceptLanguage.get(req.headers.get('Accept-Language'));

  if (!lng) lng = fallbackLng;

  // Redirect if lng in path is not supported
  if (
    !supportedLngs.some((loc) => req.nextUrl.pathname.startsWith(`/${loc}`)) &&
    !req.nextUrl.pathname.startsWith('/_next')
  ) {
    return NextResponse.redirect(new URL(`/${lng}${req.nextUrl.pathname}`, req.url));
  }

  if (req.headers.has('referer')) {
    const refererUrl = new URL(req.headers.get('referer') ?? '');
    const lngInReferer = supportedLngs.find((l) => refererUrl.pathname.startsWith(`/${l}`));
    const response = NextResponse.next();

    if (lngInReferer) response.cookies.set(cookieName, lngInReferer);

    return response;
  }

  return NextResponse.next();
}
