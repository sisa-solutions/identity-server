import Typography, { type TypographyProps } from '@mui/joy/Typography';

const PageTitle = ({ children, ...rest }: TypographyProps) => {
  return (
    <Typography level="h3" component="h1" {...rest}>
      {children}
    </Typography>
  );
};

export default PageTitle;
