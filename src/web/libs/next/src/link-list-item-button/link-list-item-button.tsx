import ListItemButton, { type ListItemButtonProps } from '@mui/joy/ListItemButton';

import Link, { type LinkProps } from '../link';

type Props = ListItemButtonProps & LinkProps;

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
    <ListItemButton {...rest}>
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
    </ListItemButton>
  );
};

export default LinkButton;
