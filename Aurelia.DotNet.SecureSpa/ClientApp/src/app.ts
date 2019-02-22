import { Aurelia } from 'aurelia-framework';
import { PLATFORM } from 'aurelia-pal';
import { Router, RouterConfiguration } from 'aurelia-router';

export class App {
  public router: Router;

  public configureRouter(config: RouterConfiguration, router: Router) {
    config.title = 'Aurelia';
    config.map([
      { route: ['', 'welcome'], name: 'welcome', moduleId: PLATFORM.moduleName('welcome'), nav: true, title: 'Welcome' },
      { route: 'users', name: 'users', moduleId: PLATFORM.moduleName('users'), nav: true, title: 'Github Users' },
      { route: 'child-router', name: 'child-router', moduleId: PLATFORM.moduleName('child-router'), nav: true, title: 'Child Router' },
      { route: 'samples', name: 'samples', moduleId: PLATFORM.moduleName('samples'), nav: true, title: 'Sample Data Tester' },
    ]);

    this.router = router;
  }
}
