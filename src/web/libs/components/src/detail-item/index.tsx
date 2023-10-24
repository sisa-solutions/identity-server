import { type ReactNode } from 'react';

import Box, { type BoxProps } from '@mui/joy/Box';

export type DetailItemProps = BoxProps & {
  label: ReactNode;
  value: ReactNode;
};

const DetailItem = ({ label, value, ...rest }: DetailItemProps) => {
  return (
    <Box {...rest}>
      <Box>{label}</Box>
      <Box>{value}</Box>
    </Box>
  );
};

export default DetailItem;
