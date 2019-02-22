import {inject} from 'aurelia-dependency-injection';
import {Project, ProjectItem, CLIOptions, UI} from 'aurelia-cli';

@inject(Project, CLIOptions, UI)
export default class ElementGenerator {
  constructor(private $safeprojectname$: Project, private options: CLIOptions, private ui: UI) { }

  execute() {
    return this.ui
      .ensureAnswer(this.options.args[0], 'What would you like to call the custom element?')
      .then(name => {
        let fileName = this.$safeprojectname$.makeFileName(name);
        let className = this.$safeprojectname$.makeClassName(name);

        this.$safeprojectname$.elements.add(
          ProjectItem.text(`${fileName}.ts`, this.generateJSSource(className)),
          ProjectItem.text(`${fileName}.html`, this.generateHTMLSource(className))
        );

        return this.$safeprojectname$.commitChanges()
          .then(() => this.ui.log(`Created ${fileName}.`));
      });
  }

  generateJSSource(className) {
return `import {bindable} from 'aurelia-framework';

export class ${className} {
  @bindable value;

  valueChanged(newValue, oldValue) {

  }
}

`
  }

  generateHTMLSource(className) {
return `<template>
  <h1>\${value}</h1>
</template>`
  }
}
