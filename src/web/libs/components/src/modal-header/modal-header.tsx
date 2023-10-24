import Box, { type BoxProps } from '@mui/joy/Box';
import ModalClose from '@mui/joy/ModalClose';
import Typography from '@mui/joy/Typography';

type ModalHeaderProps = BoxProps & {
  icon?: React.ReactNode;
  title: React.ReactNode;
  enableClose?: boolean;
};

const ModalHeader = ({ icon, title, enableClose, sx, ...rest }: ModalHeaderProps) => {
  return (
    <Box
      sx={[
        {
          display: 'flex',
          alignItems: 'center',
          gap: 2,
        },
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...rest}
    >
      {icon}
      <Typography
        component="h2"
        level="h3"
        fontSize="xl"
        sx={{
          flex: 1,
        }}
      >
        {title}
      </Typography>
      {enableClose && (
        <ModalClose
          size="sm"
          sx={{
            position: 'unset',
          }}
        />
      )}
    </Box>
  );
};

export default ModalHeader;
