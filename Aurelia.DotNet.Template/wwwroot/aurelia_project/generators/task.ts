import {inject} from 'aurelia-dependency-injection';
import {Project, ProjectItem, CLIOptions, UI} from 'aurelia-cli';

@inject(Project, CLIOptions, UI)
export default class TaskGenerator {
  constructor(private $safeprojectname$: Project, private options: CLIOptions, private ui: UI) { }

  execute() {
    return this.ui
      .ensureAnswer(this.options.args[0], 'What would you like to call the task?')
      .then(name => {
        let fileName = this.$safeprojectname$.makeFileName(name);
        let functionName = this.$safeprojectname$.makeFunctionName(name);

        this.$safeprojectname$.tasks.add(
          ProjectItem.text(`${fileName}.ts`, this.generateSource(functionName))
        );

        return this.$safeprojectname$.commitChanges()
          .then(() => this.ui.log(`Created ${fileName}.`));
      });
  }

  generateSource(functionName) {
return `import * as gulp from 'gulp';
import * as changed from 'gulp-changed';
import * as $safeprojectname$ from '../aurelia.json';

export default function ${functionName}() {
  return gulp.src($safeprojectname$.paths.???)
    .pipe(changed($safeprojectname$.paths.output, {extension: '.???'}))
    .pipe(gulp.dest($safeprojectname$.paths.output));
}

`
  }
}
