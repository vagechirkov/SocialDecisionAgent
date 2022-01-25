This repository uses [GitLFS](https://git-lfs.github.com/). Make sure that you have GitLFS installed on your machine before cloning the repository. 

# Social Decision Making


## Installation

### Import package from GitHub

1. Open the Package Manager in Unity editor: `Window -> Package Manager`

2. In the Package Manager press the `+` button to add a package and select `Add package from git URL...`.

![installation from git](Documentation~/installation_from_git.png)

3. Paste the following line in the appeared text field: `https://github.com/vagechirkov/SocialDecisionAgent.git`

4. Press the `Add` button.


### Import package from disk


## Agent models

### Social drift–diffusion model

<p align="center">
<img src="https://render.githubusercontent.com/render/math?math=%5CLarge%20L(t%2B%5CDelta%20t)%20%3D%20L(t)%2B%5Cbegin%7Bbmatrix%7D%20%5Cdelta_p%20%2B%20%5Cdelta_s%20%5Cend%7Bbmatrix%7D%5Ctimes%20%5CDelta%20t%20%2B%20%5Csqrt%7B%5CDelta%20t%7D%20%5Ctimes%20%5Cepsilon">
</p>

For more details see [1](#1).

## Machine Learning Agent training
More information [here](https://github.com/Unity-Technologies/ml-agents/blob/release_18_docs/docs/Readme.md).

### Python environment installation
```bash
conda create -n mlagents python=3.6
conda activate mlagents   
python -m pip install mlagents==0.27.0 
```

### Training Process
```bash
conda activate mlagents  
mlagents-learn Assets/SocialDecisionAgent/Samples/SocialDecisionExampleScenes/AgentBrains/training_params.yml --run-id=test 
```

Training [parameters](https://github.com/Unity-Technologies/ml-agents/blob/release_17_docs/docs/Training-Configuration-File.md#common-trainer-configurations) and an example [config files](https://github.com/Unity-Technologies/ml-agents/tree/main/config).

### View training inforation
```bash
tensorboard --logdir results --port 6006 
```


## References

<a id="1">[1]</a>
Tump, A. N., Pleskac, T. J., & Kurvers, R. H. J. M. (2020). Wise or mad crowds? The cognitive mechanisms underlying information cascades. Science Advances, 6(29), eabb0266. https://doi.org/10.1126/sciadv.abb0266

<a id="2">[2]</a>
Shinn, M., Ehrlich, D. B., Lee, D., Murray, J. D., & Seo, H. (2020). Confluence of Timing and Reward Biases in Perceptual Decision-Making Dynamics. Journal of Neuroscience, 40(38), 7326–7342. https://doi.org/10.1523/JNEUROSCI.0544-20.2020
