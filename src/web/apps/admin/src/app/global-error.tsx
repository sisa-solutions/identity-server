'use client';

type Props = {
  error: Error;
  reset: () => void;
};

const GlobalErrorPage = (props: Props) => {
  return (
    <html>
      <body>
        <h2>Something went wrong!</h2>
        <button onClick={() => props.reset()}>Try again</button>
      </body>
    </html>
  );
};

export default GlobalErrorPage;
