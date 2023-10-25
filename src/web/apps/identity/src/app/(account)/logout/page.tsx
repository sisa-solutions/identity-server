import Button from '@mui/joy/Button';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';
import { LogOutIcon } from 'lucide-react';

const LogoutPage = () => {
  return (
    <Stack direction="column" gap={2}>
      <Stack
        direction="column"
        gap={1}
        sx={{
          mb: 2,
        }}
      >
        <Typography level="h3" color="primary">
          Logout
        </Typography>
        <Typography level="body-sm">{`Do you want to logout?`}</Typography>
      </Stack>
      <Button
        type="submit"
        variant="solid"
        color="warning"
        startDecorator={<LogOutIcon />}
        sx={{ flex: 1 }}
      >
        Logout
      </Button>
    </Stack>
  );
};

export default LogoutPage;
