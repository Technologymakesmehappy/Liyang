using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UTJ {

public class GameManager
{
	// singleton
	static GameManager instance_;
        private bool IsBegan;
	public static GameManager Instance { get { return instance_ ?? (instance_ = new GameManager()); } }

	private enum GamePhase {
		Title,
		Game,
	}
	private GamePhase game_phase_ = GamePhase.Title;
	private IEnumerator enumerator_;
	private double update_time_;
	private ReplayManager replay_manager_;
	private bool replay_mode_ = false;

        public bool IsEnd = false;


        public bool CameraBackageBecomeBlack = false;


        public bool IsRankMiss = false;

	public void init()
	{
		enumerator_ = act();	// この瞬間は実行されない
		replay_manager_ = new ReplayManager();
		replay_manager_.init();
	}

	public void setReplayMode(bool flg)
	{
		replay_mode_ = flg;
	}

	public void update(float dt, double update_time)
	{
		update_time_ = update_time;
		enumerator_.MoveNext();
		replay_manager_.update(update_time, Player.Instance);
	}

	public void restart()
	{
            Debug.Log("重新开始");

            //开始播放游戏开始时背景音乐
            AudioControl.IsPlayBeganAudioClip = true;

            //隐藏排行榜
            SystemManager.Instance.IsRankMiss = true;


            //重置各种条件
            SystemManager.Instance.BecomeBlackTime = 0;

            SystemManager.Instance.time = 0;

            SystemManager.Instance.IsTipShowTip = false;

            SystemManager.Instance.timeTipBegan = 2f;

            SystemManager.Instance.IsBeganShowTipTwo = false;

            GameManager.Instance.IsEnd = false;
            GameManager.instance_.CameraBackageBecomeBlack = false;

            //重置血量
            Explosion.Instance.PlayerAttackEnemyNumber = 0;

            ////可以开始倒计时
            //BeganGameAudioControl.instance.IsBenganIenumber = true;

            BeganGameAudioControl.instance.IsEnableQifeiQiandaojishiyinxiao = false;

            //灯光控制
            BeganGameAudioControl.instance.IslightBecome = false;


            //点击开始前等待声音准备完毕
            BeganGameAudioControl.instance.WaitAudioReady = 0;

            BeganGameAudioControl.instance.IsBenganIenumber = true;


            
            //GameTimeResourcesControl._instance.IsResurceEnd = false;
            //GameTimeResourcesControl._instance.currentTime = 0;
            Debug.Log("调用一次资源加载完毕");



            SystemManager.Instance.IsBeganDanyi = false;
            replay_manager_.stopRecording();
		replay_manager_.stopPlaying(Player.Instance);
		enumerator_ = null;
		enumerator_ = act();
	}

	private IEnumerator act()
	{
		game_phase_ = GamePhase.Title;
		Player.Instance.setPhaseTitle();
		Player.Instance.setPositionXY(0f, -27f);
		SystemManager.Instance.registBgm(DrawBuffer.BGM.Stop);
		SystemManager.Instance.registMotion(DrawBuffer.Motion.Play);
		SystemManager.Instance.setFlowSpeed(0f);
		SystemManager.Instance.setSubjective(true);

		for (var w = new Utility.WaitForSeconds(4f, update_time_); !w.end(update_time_);) {yield return null; }
		Notice notice;
		{
			notice = Notice.create(-400f, 400f,
								   MySprite.Kind.GamePadPress,
								   MySprite.Type.Full,
								   false /* blink */);
		}
		var leave_time_start = update_time_;
		while (game_phase_ == GamePhase.Title) {
			bool exit_title = false;
			var elapsed_time = update_time_ - leave_time_start;

                //李林告诉我这里是游戏开始时的控制

                //if (InputManager.Instance.getButton(InputManager.Button.Fire) > 0)  此处若InputManager.Instance.getButton(InputManager.Button.Fire) > 0  则表示需要按下开火按键才可以开火，等于0时表示不需要按下也可以开火

                if (InputManager.Instance.getButton(InputManager.Button.Fire) == 0)
            {
                    GameManager.instance_.IsEnd = false;
                    exit_title = true;
				replay_manager_.startRecording(update_time_);
				replay_mode_ = false;
			} else {
				if (replay_manager_.hasRecorded()) {
					bool start_replay = false;
					if (replay_mode_) {
						if (elapsed_time > 1f) {
							start_replay = true;
						}
					} else {
						if (elapsed_time > 40f)
                            {//测试时将等待自动攻击的时间减少为10F   elapsed_time > 30f
                                start_replay = true;
							replay_mode_ = true;
						}
					}
					if (start_replay) {
                            exit_title = true; 
                            UnityEngine.Debug.Log("Camera Move");

                            //在这里走自动战斗的逻辑
						//SystemManager.Instance.setSubjective(false);
						replay_manager_.startPlaying(update_time_, Player.Instance);
					}
				}
			}
			if (exit_title) {
				game_phase_ = GamePhase.Game;   //这里是控制游戏飞机的运行
     
                SystemManager.Instance.registSound(DrawBuffer.SE.Missile);
                SystemManager.Instance.registMotion(DrawBuffer.Motion.GoodLuck);
                }
			yield return null;
		}
		notice.destroy();
		MyRandom.setSeed(123456789u);

		for (var w = new Utility.WaitForSeconds(1.5f, update_time_); !w.end(update_time_);) {yield return null; }
		Player.Instance.setPhaseStart();
		SystemManager.Instance.registBgm(DrawBuffer.BGM.Battle);
		for (var w = new Utility.WaitForSeconds(4f, update_time_); !w.end(update_time_);) {	yield return null; }
		Notice.create(0f, 0f,
					  update_time_ + 3f,
					  MySprite.Kind.GamePadPress,
					  MySprite.Type.Full,
					  true /* blink */);
		for (var w = new Utility.WaitForSeconds(1f, update_time_); !w.end(update_time_);) {	yield return null; }

		Player.Instance.setPhaseBattle();
		SystemManager.Instance.setFlowSpeed(-100f);
		while (TubeScroller.Instance.getDistance() < 100f) {
			yield return null;
		}
		
		for (var j = 0; j < 4; ++j) {
			for (var w = new Utility.WaitForSeconds(2f, update_time_); !w.end(update_time_);) {	yield return null; }
			for (var i = 0; i < 4; ++i) {
				for (var w = new Utility.WaitForSeconds(0.5f, update_time_); !w.end(update_time_);) { yield return null; }
				Enemy.create(Enemy.Type.Zako2);
			}

			if (j == 1) {
				Notice.create(-200f, 200f,
							  update_time_ + 3f,
							  MySprite.Kind.GamePadRelease,
							  MySprite.Type.Full,
							  true /* blink */);
			}
		}

		while (TubeScroller.Instance.getDistance() < 2400f) {
			yield return null;
		}

		Enemy dragon = Enemy.create(Enemy.Type.Dragon);
		SystemManager.Instance.setFlowSpeed(-10f);

		while (TubeScroller.Instance.getDistance() < 2800f) {
			for (var w = new Utility.WaitForSeconds(5f, update_time_); !w.end(update_time_);) {	yield return null; }
			for (var j = new Utility.WaitForSeconds(2f, update_time_); !j.end(update_time_);) {
				yield return null;
				Enemy.create(Enemy.Type.Zako2);
				for (var w = new Utility.WaitForSeconds(0.25f, update_time_); !w.end(update_time_);) {	yield return null; }
			}
			yield return null;
		}
		
		float flow_speed = 150f;
		SystemManager.Instance.setFlowSpeed(-150f);
		dragon.setMode(Dragon.Mode.Chase);

		for (var i = 0; i < 4; ++i) {
			for (var v = new Utility.WaitForSeconds(3f, update_time_); !v.end(update_time_);) {
				Enemy.create(Enemy.Type.Zako2);
                    //Debug.Log("a");
				for (var w = new Utility.WaitForSeconds(0.5f, update_time_); !w.end(update_time_);) { yield return null; }
			}
                //Debug.Log("b");
                for (var w = new Utility.WaitForSeconds(2f, update_time_); !w.end(update_time_);) {	yield return null; }
		}
            //Debug.Log("c");
            for (var w = new Utility.WaitForSeconds(2f, update_time_); !w.end(update_time_);) {	yield return null; }
		dragon.setMode(Dragon.Mode.Farewell);
		for (var i = 0; i < 16; ++i) {
			float rot = 30f * i;
			Shutter.create(rot, flow_speed, update_time_);
			Shutter.create(rot+180f, flow_speed, update_time_);
			for (var w = new Utility.WaitForSeconds(1f, update_time_); !w.end(update_time_);) {	yield return null; }
		}
		while (TubeScroller.Instance.getDistance() < 9400f) {
                //Debug.Log("d");
                yield return null;
		}

		dragon.setMode(Dragon.Mode.LastAttack);
            //Debug.Log("e");

            
            for (var w = new Utility.WaitForSeconds(6f, update_time_); !w.end(update_time_);) { yield return null; }
            //Debug.Log("f");
            IsEnd = true;
            CameraBackageBecomeBlack = true;

            Notice.create(0f, 0f,
					  update_time_ + 6f,
					  MySprite.Kind.Logo,
					  MySprite.Type.Full,
					  false);
            //Debug.Log("g");

            for (var w = new Utility.WaitForSeconds(6f, update_time_); !w.end(update_time_);) { yield return null; }



            SystemManager.Instance.IsGameOverNewScene = true;
            //由于不要自动开始，下面的逻辑不走，直接注释
            //SystemManager.Instance.restart();
            //SystemManager.Instance.BloodVolume = 200f;
            //EnemyBullet.AttackNumber = 0;
            //SystemManager.Instance.NowBloodVolume = 1;
        }
}
} // namespace UTJ {
