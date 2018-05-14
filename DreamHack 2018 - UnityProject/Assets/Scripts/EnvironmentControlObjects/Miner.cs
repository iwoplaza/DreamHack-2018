using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Environment;

namespace Game
{
	public class Miner : EnvironmentControlComponent
    {
		MetalMap m_metalMap;

		[SerializeField] float m_miningFrequency;

        public TilePosition MinerTilePosition { get; set; }

        [SerializeField]public int AmountGetPerMine;

		
		public override void InitializeComponent()
        {
			m_metalMap = WorldController.Instance.MainState.GameEnvironment.MetalMap;
		}

        public void StartMineRoutine()
        {
            StartCoroutine(StartMining());
        }
		
		public IEnumerator StartMining()
        {
            if(GetComponent<AnimatedObject>() != null)
            {
                GetComponent<AnimatedObject>().Activate();
            }
            float miningFreq = 0;
            while(m_metalMap.MetalAmountAt(MinerTilePosition) > 0)
            {
                if(miningFreq <= 0){
                    int curAmount = (int)m_metalMap.MetalAmountAt(MinerTilePosition);
                    int newValue = (int)m_metalMap.MetalAmountAt(MinerTilePosition) - AmountGetPerMine;
                    m_metalMap.SetMetalAmountAt(MinerTilePosition, (ushort)newValue);
                    newValue = (int)m_metalMap.MetalAmountAt(MinerTilePosition);

                    WorldController.Instance.MainState.ItemStorage.Add(curAmount - newValue, Items.Item.METAL);

                    miningFreq = m_miningFrequency;
                }
                miningFreq -= Time.deltaTime * WorldController.Instance.MainState.TimeSystem.TimeMultiplier;

                yield return new WaitForEndOfFrame();
            }

            if(GetComponent<AnimatedObject>() != null)
            {
                GetComponent<AnimatedObject>().Deactivate();
            }
        }

		public override void UpdateComponent()
        {

		}
	}
}
