# Helm Deployments

## Prerequisites

The following requirements must be met before running.

1. Access to the kubernetes cluster
2. The following software utilities: kubectl, helm, helmfile.


## Deploying Helm Charts

All the configuration is for deploying the helm charts is found in the [helmfile.yaml](helmfile.yaml) file.

```bash
helmfile --environment dev sync
```

## Destroying Helm Charts

This will remove all charts.

```bash
helmfile --environment dev destroy
```

