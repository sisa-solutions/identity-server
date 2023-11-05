import { type ReactNode } from 'react';

import BaseBreadcrumbs, { type BreadcrumbsProps } from '@mui/joy/Breadcrumbs';
import Typography from '@mui/joy/Typography';

import { ChevronRightIcon, HomeIcon } from 'lucide-react';

import { Link } from '@sisa/components';

export type BreadcrumbItem = {
  title: ReactNode;
  url?: string;
};

type Props = Omit<BreadcrumbsProps, 'children'> & {
  items: Array<BreadcrumbItem>;
};

const Breadcrumbs = ({ items, sx, ...props }: Props) => {
  return (
    <BaseBreadcrumbs
      size="sm"
      separator={<ChevronRightIcon />}
      sx={[
        {
          px: 2,
        },
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...props}
    >
      <Link href="/dashboard" level="body-sm">
        <HomeIcon />
      </Link>
      {items.map(({ title, url }, index) => {
        if (url) {
          return (
            <Link key={index} href={url} level="body-sm">
              {title}
            </Link>
          );
        }

        return (
          <Typography key={index} level="body-sm">
            {title}
          </Typography>
        );
      })}
    </BaseBreadcrumbs>
  );
};

export default Breadcrumbs;
