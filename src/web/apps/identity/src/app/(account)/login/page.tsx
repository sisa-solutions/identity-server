'use client';

import Button from '@mui/joy/Button';
import Card from '@mui/joy/Card';
import Checkbox from '@mui/joy/Checkbox';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';
import Divider from '@mui/joy/Divider';
import ButtonGroup from '@mui/joy/ButtonGroup';

import { GithubIcon, LogInIcon, TwitterIcon } from 'lucide-react';

import {
  FormActions,
  FormContainer,
  PasswordField,
  TextField,
  useForm,
  yup,
  yupResolver,
} from '@sisa/form';
import { Link } from '@sisa/next';
import { GoogleIcon } from '@sisa/icons';
import { Tooltip } from '@mui/joy';
import { useSearchParams } from 'next/navigation';

const isClient = typeof window !== 'undefined';

const LoginPage = () => {
  const xsrfToken = isClient
    ? document.cookie
        .split(';')
        .find((cookie) => cookie.startsWith('x-xsrf-token'))
        ?.split('=')[1]
    : '';
  const searchParams = useSearchParams();
  const returnUrl = searchParams.get('return_url');

  const validationSchema = yup.object({
    username: yup.string().required().min(6).max(50).label('Email or Username'),
    password: yup
      .string()
      .required()
      .min(8)
      .max(20)
      .label('Password')
      .test(
        'password',
        'Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.',
        (value) => {
          return /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])/.test(value);
        }
      ),
    rememberMe: yup.boolean().label('Remember me'),
  });

  type FormValues = yup.InferType<typeof validationSchema>;

  const { control, handleSubmit, register } = useForm<FormValues>({
    defaultValues: {
      username: '',
      password: '',
      rememberMe: false,
    },
    resolver: yupResolver(validationSchema),
    reValidateMode: 'onBlur',
  });

  const onSubmit = handleSubmit(async (data: FormValues) => {
    console.log(data);

    const body = new URLSearchParams();

    body.append('username', data.username);
    body.append('password', data.password);
    body.append('rememberMe', (data.rememberMe ?? false).toString());

    const response = await fetch(`/api/v1/account/login?return_url=${returnUrl}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
        'x-xsrf-token': xsrfToken ?? '',
      },
      body,
    });

    if (response.status === 200) {
      const data = await response.json();

      window.location.href = data.redirectUrl;
    }

    console.log(response);
  });

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
          Login
        </Typography>
        <Card variant="soft">
          <Typography level="body-sm">{`Let's get you logged in.`}</Typography>
        </Card>
      </Stack>

      <FormContainer orientation="vertical">
        <TextField control={control} name="username" label="Email or Username" required />
        <PasswordField
          control={control}
          helperMessage={
            <Link href="/forgot-password" color="primary" underline="hover">
              Forgot password?
            </Link>
          }
          name="password"
          label="Password"
          required
          sx={{
            '& .form-label-group': {
              display: 'flex',
              justifyContent: 'space-between',
            },
          }}
        />
        <Checkbox label="Remember me" {...register('rememberMe')} />
        <FormActions display="flex" flex={1} mt={2}>
          <Button
            type="submit"
            variant="solid"
            color="primary"
            startDecorator={<LogInIcon />}
            sx={{ flex: 1 }}
            onClick={onSubmit}
          >
            Login
          </Button>
        </FormActions>
      </FormContainer>

      <Divider>or continue with</Divider>

      <ButtonGroup
        orientation="horizontal"
        spacing={2}
        sx={{
          '& > button': {
            flex: 1,
          },
        }}
        variant="outlined"
        color="primary"
      >
        <Tooltip title="Google">
          <Button>
            <GoogleIcon />
          </Button>
        </Tooltip>
        <Tooltip title="Google">
          <Button>
            <GithubIcon />
          </Button>
        </Tooltip>
        <Button>
          <TwitterIcon />
        </Button>
      </ButtonGroup>

      {/* <Typography level="body-sm" textAlign="right" mt={2}>
        {`Don't have an account? `}
        <Link href="/register" color="primary" underline="always">
          Register here
        </Link>
      </Typography> */}
    </Stack>
  );
};

export default LoginPage;
