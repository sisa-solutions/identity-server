import NextLink, { type LinkProps as NextLinkProps } from 'next/link';
import BaseLink, { type LinkProps as BaseLinkProps } from '@mui/joy/Link';

// import { randomId } from '@sisa/utils';

export type LinkProps = Omit<BaseLinkProps<'a'>, 'components' | 'href'> &
  // @ts-ignore
  Omit<NextLinkProps, 'passHref' | 'legacyBehavior' | 'as'> & {
    disableCache?: boolean;
  };

const Link = (props: LinkProps) => {
  const { children, disableCache = false, href, ...rest } = props;

  // if (disableCache) {
  //   return (
  //     <BaseLink
  //       component={NextLink}
  //       href={`${href}?__s=${randomId()}`}
  //       {...rest}
  //       textColor="inherit"
  //       fontSize="inherit"
  //       fontWeight="inherit"
  //       fontFamily="inherit"
  //     >
  //       {children}
  //     </BaseLink>
  //   );
  // }

  return (
    <BaseLink
      component={NextLink}
      href={href}
      {...rest}
      textColor="inherit"
      fontSize="inherit"
      fontWeight="inherit"
      fontFamily="inherit"
    >
      {children}
    </BaseLink>
  );
};

export default Link;
