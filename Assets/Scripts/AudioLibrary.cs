using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Academy.HoloToolkit.Unity;

//Library that holds all of our audioclip sounds

public class AudioLibrary : Singleton<AudioLibrary> 
{
	//audio clips declared
	public AudioClip vitalsWindow;
	public AudioClip proceduresSFX;
	public AudioClip onboardingSFX;
	public AudioClip instructionsSFX;
	public AudioClip completionStepSFX;
	public AudioClip TadaSFX;
	public AudioClip AlertSFX;

	//open close panel menus
	public AudioClip openMenuSFX;
	public AudioClip closeMenuSFX;







	public static void VitalsWindowSFX()
	{
		//AudioManager.PlaySound();
	}
	public static void ProceduresSFX()
	{
		//AudioManager.PlaySound();
	}
	public static void OnboardingSFX()
	{
		//AudioManager.PlaySound();
	}
	public static void InstructionsSFX()
	{
		//AudioManager.PlaySound();
	}
	public static void CompletionStepSFX()
	{
		AudioManager.PlaySound(inst.completionStepSFX);
	}
	public static void OpenMenuSFX()
	{
		AudioManager.PlaySound(inst.openMenuSFX);
	}
	public static void CloseMenuSFX()
	{
		AudioManager.PlaySound(inst.closeMenuSFX);
	}
	public static void Tada()
	{
		AudioManager.PlaySound (inst.TadaSFX);
	}
	public static void AlertSound()
	{
		AudioManager.PlaySound (inst.AlertSFX);
	}




}
