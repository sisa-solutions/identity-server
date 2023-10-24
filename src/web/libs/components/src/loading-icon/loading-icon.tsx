import { memo } from 'react';

import { LoaderIcon, type LucideProps } from 'lucide-react';

const LoadingIcon = memo((props: LucideProps) => {
  return <LoaderIcon className="animate-spin" {...props} />;
});

LoadingIcon.displayName = 'LoadingIcon';

export default LoadingIcon;
