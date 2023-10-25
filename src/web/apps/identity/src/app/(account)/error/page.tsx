import Card from '@mui/joy/Card';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';

const ErrorPage = () => {
  return (
    <Stack direction="column" gap={2}>
      <Stack
        direction="column"
        gap={1}
        sx={{
          mb: 2,
        }}
      >
        <Typography level="h3" color="danger">
          Error
        </Typography>
        <Card variant="soft">
          <Typography level="body-sm">Something went wrong. Please try again later.</Typography>
        </Card>
      </Stack>
    </Stack>
  );
};

export default ErrorPage;
