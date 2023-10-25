import Chip, { type ChipProps } from '@mui/joy/Chip';

import Link, { type LinkProps } from '../link';

type Props = ChipProps & LinkProps;

const LinkChip = ({
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
    <Chip {...rest}>
      <Link
        href={href}
        replace={replace}
        prefetch={prefetch}
        locale={locale}
        scroll={scroll}
        shallow={shallow}
        underline={underline}
      >
        {children}
      </Link>
    </Chip>
  );
};

export default LinkChip;
