import Card from '@mui/joy/Card';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';

const ForgotPasswordConfirmationPage = () => {
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
          Forgot password confirmation
        </Typography>
        <Card variant="soft">
          <Typography level="body-sm">
            {`We've sent you an email with a link to reset your password.`}
          </Typography>
        </Card>
      </Stack>
    </Stack>
  );
};

export default ForgotPasswordConfirmationPage;
