'use client';

import AspectRatio from '@mui/joy/AspectRatio';
import Card from '@mui/joy/Card';
import CardContent from '@mui/joy/CardContent';
import CardOverflow from '@mui/joy/CardOverflow';
import Divider from '@mui/joy/Divider';
import Grid from '@mui/joy/Grid';
import IconButton from '@mui/joy/IconButton';
import Typography from '@mui/joy/Typography';

import { Chart, PageContent } from '@sisa/components';
import { MoreHorizontalIcon, TrendingUpIcon } from 'lucide-react';

const WelcomePage = () => {
  return (
    <PageContent>
      <Grid spacing={2} container>
        {[1, 2, 3, 4].map((item) => (
          <Grid key={item} xs={6} md={4} lg={3}>
            <Card>
              <CardContent
                orientation="horizontal"
                sx={{
                  alignItems: 'center',
                  gap: 1,
                }}
              >
                <Typography
                  textColor="text.icon"
                  sx={{
                    flex: 'auto',
                  }}
                >
                  Visitors
                </Typography>
                <IconButton variant="plain" color="neutral" size="sm" sx={{ mt: -1, mr: -1 }}>
                  <MoreHorizontalIcon />
                </IconButton>
              </CardContent>

              <CardContent
                orientation="horizontal"
                sx={{
                  alignItems: 'center',
                }}
              >
                <Typography level="h3" component="span">
                  42,000
                </Typography>
                <Typography level="body-sm" color="success" endDecorator={<TrendingUpIcon />}>
                  + 30%
                </Typography>
              </CardContent>
              <CardOverflow>
                <AspectRatio ratio="5" variant="plain">
                  <Chart
                    option={{
                      grid: {
                        left: '12%',
                        top: '8%',
                        right: '4%',
                        bottom: '12%',
                      },
                      xAxis: {
                        type: 'category',
                        data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
                      },
                      yAxis: {
                        type: 'value',
                      },
                      series: [
                        {
                          data: [120, 200, 150, 80, 70, 110, 130],
                          type: 'bar',
                          showBackground: true,
                          backgroundStyle: {
                            color: 'rgba(180, 180, 180, 0.2)',
                          },
                        },
                      ],
                    }}
                  />
                </AspectRatio>
              </CardOverflow>
            </Card>
          </Grid>
        ))}
      </Grid>
      <Grid spacing={2} container>
        <Grid xs={6} md={4} lg={3}>
          <Card
            sx={{
              '--Card-padding': 0,
              gap: 0,
            }}
          >
            <CardContent
              sx={{
                px: 2,
                py: 1,
              }}
            >
              <Typography level="title-md">Sales</Typography>
            </CardContent>
            <Divider
              sx={(theme) => ({
                '--Divider-thickness': '2px',
                '--Divider-lineColor': theme.palette.primary[400],
              })}
            />
            <AspectRatio variant="plain">
              <Chart
                option={{
                  grid: {
                    left: '12%',
                    top: '8%',
                    right: '4%',
                    bottom: '12%',
                  },
                  xAxis: {
                    type: 'category',
                    data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
                  },
                  yAxis: {
                    type: 'value',
                  },
                  series: [
                    {
                      data: [120, 200, 150, 80, 70, 110, 130],
                      type: 'bar',
                      showBackground: true,
                      backgroundStyle: {
                        color: 'rgba(180, 180, 180, 0.2)',
                      },
                    },
                  ],
                }}
              />
            </AspectRatio>
          </Card>
        </Grid>
        <Grid xs={6} md={4} lg={3}>
          <Card
            sx={{
              '--Card-padding': 0,
              gap: 0,
            }}
          >
            <CardContent
              sx={{
                px: 2,
                py: 1,
              }}
            >
              <Typography level="title-md">Sales</Typography>
            </CardContent>
            <Divider
              sx={(theme) => ({
                '--Divider-thickness': '2px',
                '--Divider-lineColor': theme.palette.primary[400],
              })}
            />
            <AspectRatio variant="plain">
              <Chart
                option={{
                  grid: {
                    left: '12%',
                    top: '8%',
                    right: '4%',
                    bottom: '12%',
                  },
                  series: [
                    {
                      name: 'Nightingale Chart',
                      type: 'pie',
                      radius: [20, 80],
                      center: ['50%', '50%'],
                      roseType: 'area',
                      itemStyle: {
                        borderRadius: 8,
                      },
                      data: [
                        { value: 40, name: 'rose 1' },
                        { value: 38, name: 'rose 2' },
                        { value: 32, name: 'rose 3' },
                        { value: 30, name: 'rose 4' },
                        { value: 28, name: 'rose 5' },
                        { value: 26, name: 'rose 6' },
                        { value: 22, name: 'rose 7' },
                        { value: 18, name: 'rose 8' },
                      ],
                    },
                  ],
                }}
              />
            </AspectRatio>
          </Card>
        </Grid>
        <Grid xs={6} md={4} lg={3}>
          <Card
            sx={{
              '--Card-padding': 0,
              gap: 0,
            }}
          >
            <CardContent
              sx={{
                px: 2,
                py: 1,
              }}
            >
              <Typography level="title-md">Sales</Typography>
            </CardContent>
            <Divider
              sx={(theme) => ({
                '--Divider-thickness': '2px',
                '--Divider-lineColor': theme.palette.primary[400],
              })}
            />
            <AspectRatio variant="plain">
              <Chart
                option={{
                  grid: {
                    left: '12%',
                    top: '8%',
                    right: '4%',
                    bottom: '12%',
                  },
                  xAxis: {
                    type: 'category',
                    boundaryGap: false,
                    data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
                  },
                  yAxis: {
                    type: 'value',
                  },
                  series: [
                    {
                      name: 'Email',
                      type: 'line',
                      stack: 'Total',
                      data: [120, 132, 101, 134, 90, 230, 210],
                    },
                    {
                      name: 'Union Ads',
                      type: 'line',
                      stack: 'Total',
                      data: [220, 182, 191, 234, 290, 330, 310],
                    },
                    {
                      name: 'Video Ads',
                      type: 'line',
                      stack: 'Total',
                      data: [150, 232, 201, 154, 190, 330, 410],
                    },
                    {
                      name: 'Direct',
                      type: 'line',
                      stack: 'Total',
                      data: [320, 332, 301, 334, 390, 330, 320],
                    },
                    {
                      name: 'Search Engine',
                      type: 'line',
                      stack: 'Total',
                      data: [820, 932, 901, 934, 1290, 1330, 1320],
                    },
                  ],
                }}
              />
            </AspectRatio>
          </Card>
        </Grid>
        <Grid xs={6} md={4} lg={3}>
          <Card
            sx={{
              '--Card-padding': 0,
              gap: 0,
            }}
          >
            <CardContent
              sx={{
                px: 2,
                py: 1,
              }}
            >
              <Typography level="title-md">Sales</Typography>
            </CardContent>
            <Divider
              sx={(theme) => ({
                '--Divider-thickness': '2px',
                '--Divider-lineColor': theme.palette.primary[400],
              })}
            />
            <AspectRatio variant="plain">
              <Chart
                option={{
                  grid: {
                    left: '12%',
                    top: '8%',
                    right: '4%',
                    bottom: '12%',
                  },
                  xAxis: {
                    type: 'category',
                    splitLine: { show: false },
                    data: ['Total', 'Rent', 'Utilities', 'Transportation', 'Meals', 'Other'],
                  },
                  yAxis: {
                    type: 'value',
                  },
                  series: [
                    {
                      name: 'Placeholder',
                      type: 'bar',
                      stack: 'Total',
                      itemStyle: {
                        borderColor: 'transparent',
                        color: 'transparent',
                      },
                      emphasis: {
                        itemStyle: {
                          borderColor: 'transparent',
                          color: 'transparent',
                        },
                      },
                      data: [0, 1700, 1400, 1200, 300, 0],
                    },
                    {
                      name: 'Life Cost',
                      type: 'bar',
                      stack: 'Total',
                      label: {
                        show: true,
                        position: 'inside',
                      },
                      data: [2900, 1200, 300, 200, 900, 300],
                    },
                  ],
                }}
              />
            </AspectRatio>
          </Card>
        </Grid>
      </Grid>
    </PageContent>
  );
};

export default WelcomePage;
