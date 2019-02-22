import {inject} from 'aurelia-dependency-injection';
import {Project, ProjectItem, CLIOptions, UI} from 'aurelia-cli';

@inject(Project, CLIOptions, UI)
export default class BindingBehaviorGenerator {
  constructor(private $safeprojectname$: Project, private options: CLIOptions, private ui: UI) { }

  execute() {
    return this.ui
      .ensureAnswer(this.options.args[0], 'What would you like to call the binding behavior?')
      .then(name => {
        let fileName = this.$safeprojectname$.makeFileName(name);
        let className = this.$safeprojectname$.makeClassName(name);

        this.$safeprojectname$.bindingBehaviors.add(
          ProjectItem.text(`${fileName}.ts`, this.generateSource(className))
        );

        return this.$safeprojectname$.commitChanges()
          .then(() => this.ui.log(`Created ${fileName}.`));
      });
  }

  generateSource(className) {
return `export class ${className}BindingBehavior {
  bind(binding, source) {

  }

  unbind(binding, source) {

  }
}

`
  }
}
