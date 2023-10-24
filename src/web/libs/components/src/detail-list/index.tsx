import Box, { type BoxProps } from '@mui/joy/Box';

export type DetailListProps = BoxProps & {
  orientation?: 'vertical' | 'horizontal';
};

const DetailList = ({
  orientation = 'vertical',
  children,
  sx,
  ...rest
}: DetailListProps) => {
  return (
    <Box
      sx={[
        {
          ...(orientation === 'horizontal'
            ? {
                display: 'grid',
                '& > div': {
                  display: {
                    sm: 'contents',
                  },
                },
                '& > hr': {
                  gridColumn: '1/-1',
                },
                gridTemplateColumns: {
                  xs: '100%',
                  sm: 'minmax(80px, 240px) 1fr',
                },
                rowGap: {
                  xs: 2,
                  sm: 2.5,
                },
                columnGap: {
                  xs: 2,
                  sm: 2.5,
                },
              }
            : {
                display: 'flex',
                flexDirection: 'column',
                gap: 2,

                '& > div': {
                  display: 'inherit',
                },
              }),
        },
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...rest}
    >
      {children}
    </Box>
  );
};

export default DetailList;
