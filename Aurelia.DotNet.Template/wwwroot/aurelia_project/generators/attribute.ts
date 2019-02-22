import {inject} from 'aurelia-dependency-injection';
import {Project, ProjectItem, CLIOptions, UI} from 'aurelia-cli';

@inject(Project, CLIOptions, UI)
export default class AttributeGenerator {
  constructor(private $safeprojectname$: Project, private options: CLIOptions, private ui: UI) { }

  execute() {
    return this.ui
      .ensureAnswer(this.options.args[0], 'What would you like to call the custom attribute?')
      .then(name => {
        let fileName = this.$safeprojectname$.makeFileName(name);
        let className = this.$safeprojectname$.makeClassName(name);

        this.$safeprojectname$.attributes.add(
          ProjectItem.text(`${fileName}.ts`, this.generateSource(className))
        );

        return this.$safeprojectname$.commitChanges()
          .then(() => this.ui.log(`Created ${fileName}.`));
      });
  }

  generateSource(className) {
return `import {autoinject} from 'aurelia-framework';

@autoinject()
export class ${className}CustomAttribute {
  constructor(private element: Element) { }

  valueChanged(newValue, oldValue) {

  }
}

`
  }
}
