import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';

const ResetPasswordConfirmationPage = () => {
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
          Reset password confirmation
        </Typography>
        <Typography level="body-sm">{`Your password has been reset.`}</Typography>
      </Stack>
    </Stack>
  );
};

export default ResetPasswordConfirmationPage;
