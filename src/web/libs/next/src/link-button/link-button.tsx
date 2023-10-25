import Button, { type ButtonProps } from '@mui/joy/Button';

import Link, { type LinkProps } from '../link';

type Props = ButtonProps & LinkProps;

const LinkButton = ({
  children,
  href,
  replace,
  prefetch,
  locale,
  scroll,
  shallow,
  underline,
  ...rest
}: Props) => {
  return (
    <Button {...rest}>
      <Link
        href={href}
        replace={replace}
        prefetch={prefetch}
        locale={locale}
        scroll={scroll}
        shallow={shallow}
        underline={underline}
        overlay
      >
        {children}
      </Link>
    </Button>
  );
};

export default LinkButton;
