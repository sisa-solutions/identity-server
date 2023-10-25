import Card from '@mui/joy/Card';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';
import { LinkButton } from '@sisa/next';
import { LogInIcon } from 'lucide-react';

const WelcomePage = () => {
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
          Welcome to Sisa Identity
        </Typography>
        <Card variant="soft">
          <Typography level="body-sm">
            {`You are currently logged in as a guest. Please sign in or create an account to continue.`}
          </Typography>
        </Card>
      </Stack>
      <LinkButton
        href="/login"
        variant="solid"
        color="primary"
        // flexGrow={1}
        underline="none"
        startDecorator={<LogInIcon />}
      >
        Sign in
      </LinkButton>
    </Stack>
  );
};

export default WelcomePage;
