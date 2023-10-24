import { memo } from 'react';

import dynamic from 'next/dynamic';

import { LucideProps } from 'lucide-react';
import dynamicIconImports from 'lucide-react/dynamicIconImports';

import LoadingIcon from '../loading-icon';

interface IconProps extends Omit<LucideProps, 'ref'> {
  name: keyof typeof dynamicIconImports;
}

const Icon = memo(({ name, ...props }: IconProps) => {
  const LucideIcon = dynamic(dynamicIconImports[name], {
    ssr: true,
    loading: () => <LoadingIcon />,
  });

  return <LucideIcon {...props} />;
});

Icon.displayName = 'Icon';

export default Icon;
