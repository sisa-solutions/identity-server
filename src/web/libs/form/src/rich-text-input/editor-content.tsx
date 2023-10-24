import { forwardRef } from 'react';

import Box, { type BoxProps } from '@mui/joy/Box';

const EditorContent = forwardRef<HTMLDivElement>((props: BoxProps, ref) => {
  // @ts-ignore
  const { ownerState, ...other } = props;

  return <Box ref={ref} {...other} />;
});

EditorContent.displayName = 'EditorContent';

export default EditorContent;
