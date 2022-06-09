package main

import (
	"mime"
	"net/http"
	"os"
	"path"
	"path/filepath"
	"time"

	"github.com/gin-contrib/location"
	"github.com/gin-gonic/gin"
	"github.com/sirupsen/logrus"
	"github.com/urfave/cli/v2"
	"golang.org/x/sync/errgroup"
)

var (
  g errgroup.Group
  apiServerAddrFlagName string = "addr"
)

func router() http.Handler {
  e := gin.New()
  e.Use(gin.Logger())
  e.Use(gin.Recovery())

  mime.AddExtensionType(".js", "application/javascript")

  e.Use(location.New(location.Config{
    Scheme: "https",
    Host: "devqa",
    Base: "/design",
  }))

  e.NoRoute(func(c *gin.Context) {
    dir, file := path.Split(c.Request.RequestURI)
    ext := filepath.Ext(file)
    if file == "" || ext == "" {
      c.File("./ui/dist/ds-docs/index.html")
    } else {
      c.File("./ui/dist/ds-docs/" + path.Join(dir, file))
    }
  })

  e.GET("/ping", func(c *gin.Context) {
    url := location.Get(c)

    println(url.Scheme, url.Host, url.Path)

    c.JSON(200, gin.H{
      "message": "pong",
    })
  })

  return e
}

func app() *cli.App {
  return &cli.App {
    Name: "api-server",
    Usage: "Web Server",
    Commands: []*cli.Command{
      apiServerCmd(),
    },
  }
}

func apiServerCmd() *cli.Command {
  return &cli.Command {
    Name: "Start",
    Usage: "starts the web server",
    Flags: []cli.Flag {
      &cli.StringFlag{Name: apiServerAddrFlagName, EnvVars: []string{"API_SERVER_ADDR"}},
    },
    Action: func(c *cli.Context) error {
      addr := c.String(apiServerAddrFlagName)

      server := &http.Server{
        Addr: addr,
        Handler: router(),
        ReadTimeout: 5 * time.Second,
        WriteTimeout: 10 * time.Second,
      }

      g.Go(func() error {
        return server.ListenAndServe()
      })

      err := g.Wait()

      if err != nil {
        logrus.Error(err)
      }

      return err
    },
  }
}

func main() {
  if err := app().Run(os.Args); err != nil {
    logrus.WithError(err).Fatal("coult not run application")
  }
}
