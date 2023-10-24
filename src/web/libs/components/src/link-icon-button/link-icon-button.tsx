import IconButton, { type IconButtonProps } from '@mui/joy/IconButton';

import Link, { type LinkProps } from '../link';

type Props = IconButtonProps & LinkProps;

const LinkIconButton = ({
  children,
  href,
  replace,
  prefetch,
  locale,
  scroll,
  shallow,
  ...rest
}: Props) => {
  return (
    <IconButton {...rest}>
      <Link
        href={href}
        replace={replace}
        prefetch={prefetch}
        locale={locale}
        scroll={scroll}
        shallow={shallow}
        overlay={true}
      >
        {children}
      </Link>
    </IconButton>
  );
};

export default LinkIconButton;
