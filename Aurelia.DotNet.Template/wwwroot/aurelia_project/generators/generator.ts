import {inject} from 'aurelia-dependency-injection';
import {Project, ProjectItem, CLIOptions, UI} from 'aurelia-cli';

@inject(Project, CLIOptions, UI)
export default class GeneratorGenerator {
  constructor(private $safeprojectname$: Project, private options: CLIOptions, private ui: UI) { }

  execute() {
    return this.ui
      .ensureAnswer(this.options.args[0], 'What would you like to call the generator?')
      .then(name => {
        let fileName = this.$safeprojectname$.makeFileName(name);
        let className = this.$safeprojectname$.makeClassName(name);

        this.$safeprojectname$.generators.add(
          ProjectItem.text(`${fileName}.ts`, this.generateSource(className))
        );

        return this.$safeprojectname$.commitChanges()
          .then(() => this.ui.log(`Created ${fileName}.`));
      });
  }

  generateSource(className) {
return `import {autoinject} from 'aurelia-dependency-injection';
import {Project, ProjectItem, CLIOptions, UI} from 'aurelia-cli';

@autoinject()
export default class ${className}Generator {
  constructor(private $safeprojectname$: Project, private options: CLIOptions, private ui: UI) { }

  execute() {
    return this.ui
      .ensureAnswer(this.options.args[0], 'What would you like to call the new item?')
      .then(name => {
        let fileName = this.$safeprojectname$.makeFileName(name);
        let className = this.$safeprojectname$.makeClassName(name);

        this.$safeprojectname$.elements.add(
          ProjectItem.text(\`\${fileName}.ts\`, this.generateSource(className))
        );

        return this.$safeprojectname$.commitChanges()
          .then(() => this.ui.log(\`Created \${fileName}.\`));
      });
  }

  generateSource(className) {
return \`import {bindable} from 'aurelia-framework';

export class \${className} {
  @bindable value;

  valueChanged(newValue, oldValue) {

  }
}

\`
  }
}

`
  }
}
